/* 
 * Author:    Thomas Baart
 * Contact:   mail@thomasbaart.nl
 * Date:      26-02-2014
 *
 * Tests:     ./wizardSpecs.js
 * Passes:    Yes, 26-02-2014
 */

(function () {
    'use strict';

    var moduleId = 'components.wizard',
        serviceId = 'wizard';

    angular.module(moduleId, [])

    ////////////////////////////////////////////////////////////////////////////////////
    // COMPONENTS.WIZARD
    ////////////////////////////////////////////////////////////////////////////////////
    .factory(serviceId, function () {
        return function () {
            ////////////////////////////////////////////////////////////////////////////
            // PRIVATE VARIABLES

            var mod          = {}
              , m_options    = {}
              , m_steps      = []
              , m_index      =  0
              , ERROR_STATUS = -1;

            ////////////////////////////////////////////////////////////////////////////
            // PRIVATE FUNCTIONS

            /* Returns true when the index is within bounds. */
            function isValidIndex(index) {
                if (index < 0 || index > m_steps.length - 1) {
                    return false;
                }

                return true;
            }

            /* Returns the index of the next enabled step. */
            function nextIndex() {
                for (var i = m_index + 1; i < m_steps.length; i += 1) {
                    if (m_steps[i].enabled) {
                        return i;
                    }
                }

                //if (m_index < m_steps.length - 1) {
                //    return m_index + 1;
                //}

                return ERROR_STATUS;
            }

            // returns the previous index
            function prevIndex() {
                for (var i = m_index - 1; i >= 0; i -= 1) {
                    if (m_steps[i].enabled) {
                        return i;
                    }
                }

                //if (m_index > 0) {
                //    return m_index - 1;
                //}

                return ERROR_STATUS;
            }

            // sets the enabled status
            function enabled(index, value) {
                if (!isValidIndex(index)) {
                    throw new Error('Index out of range');
                }

                if (!_.isBoolean(value)) {
                    throw new TypeError('Expected a boolean for param "value"');
                }

                m_steps[index].enabled = value;
            };

            function mixedIndexToNumber(index) {
                if (_.isNumber(index)) {
                    if (!isValidIndex(index)) {
                        throw new Error('Index out of range');
                    }

                    return index;
                } else if (_.isString(index)) {
                    var idx = _.indexOf(_.pluck(m_steps, 'code'), index);

                    if (idx >= 0) {
                        return idx;
                    } else {
                        throw new Error('Object not found with code ' + index);
                    }
                } else {
                    throw new TypeError('Invalid argument');
                }
            }
            
            ////////////////////////////////////////////////////////////////////////////
            // PUBLIC FUNCTIONS

            //--------------------------------------------------------------------------
            // init(steps, options)
            //
            // steps
            //   enabled    Boolean. Enable this step. Disabled steps will not count
            //                toward the step total, and will be skipped when
            //                navigating with next() and prev(). This defaults
            //                to 'true'.
            //   visited    Set to true when passed through with next(). This defaults
            //                to 'false'.
            //   isValid    Function. Should return true when the step is valid, so
            //                that mayProceed() will return true when the current
            //                step and all before it are valid.
            //
            // options
            //   maySubmit  Function. Should return true when the wizard may submit,
            //                so that maySubmit() may return this value. This defaults
            //                to 'true'.
            //   onSubmit   Function. Is called when submit() is called.
            //   onNext     Function. Is called when next() is called.
            //--------------------------------------------------------------------------
            mod.init = function (steps, options) {
                if (!_.isArray(steps)) {
                    throw new TypeError('Expected array but received ' + steps);
                }

                if (_.isUndefined(options)) {
                    options = {};
                }

                if (!_.isObject(options)) {
                    throw new TypeError('Expected object type for options but received other.');
                }

                // Add default handlers and properties to steps
                m_steps = _.map(steps, function (val) {
                    if (!_.isObject(val)) {
                        throw new TypeError('Expected array of objects but received otherwise');
                    }

                    if (!_.has(val, 'step')) {
                        throw new Error('Expected step object to contain property "step"');
                    }

                    var defaultStep = {
                        enabled: true,
                        visited: false,
                        isValid: function () { return true; }
                    };

                    return _.defaults(val, defaultStep);
                });
                
                m_steps[m_index].visited = true;

                // Add default handlers
                var defaultOptions = {
                    maySubmit: function () { return true },
                    onSubmit: function () { /* do nothing */ },
                    onNext: function () { /* do nothing */ }
                };

                m_options = _.defaults(options, defaultOptions);
            };

            /* Returns the current step object. */
            mod.current = function () {
                if (!isValidIndex(m_index)) {
                    return null;
                }

                return m_steps[m_index];
            };

            /* Returns all enabled steps. */
            mod.steps = function () {
                return _.where(m_steps, { 'enabled': true });
            };

            /* Returns true when the current step and those before it 
               all return true for isValid. */
            mod.mayProceed = function () {
                for (var i = 0; i <= m_index; i += 1) {
                    if (m_steps[i].enabled && !m_steps[i].isValid()) {
                        console.log(m_steps[i].step + ' ' + 'was not valid');
                        return false;
                    }
                }

                return true;
            };
            
            // whether there is a next step
            mod.hasNext = function () {
                return nextIndex() !== ERROR_STATUS && mod.mayProceed();
            };

            // move to the next step
            mod.next = function () {
                if (nextIndex() !== ERROR_STATUS && mod.mayProceed()) {
                    m_index = nextIndex();

                    m_steps[m_index].visited = true;

                    m_options.onNext();
                }
            };

            // whether there is a previous step
            mod.hasPrev = function () {
                return prevIndex() !== ERROR_STATUS;
            };

            // move to the previous step
            mod.prev = function () {
                if (prevIndex() !== ERROR_STATUS) {
                    m_index = prevIndex();
                }
            };

            // TODO: tests
            mod.jump = function (index) {
                m_index = mixedIndexToNumber(index);
            };

            // disable a step at index
            mod.disable = function (index) {
                index = mixedIndexToNumber(index);

                enabled(index, false);
            };

            // enable a step at index
            mod.enable = function (index) {
                index = mixedIndexToNumber(index);

                enabled(index, true);
            };

            mod.maySubmit = function () {
                return m_options.maySubmit();
            };

            mod.submit = function () {
                if (mod.maySubmit()) {
                    m_options.onSubmit();
                } else {
                    console.log('submit was called but maySubmit precondition was not satisfied.');
                }
            };

            // get internal values
            mod.diagnostics = function () {
                return {
                    steps: m_steps,
                    index: m_index
                };
            };

            return mod;
        };
    });
})();