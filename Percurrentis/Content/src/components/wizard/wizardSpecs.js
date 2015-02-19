/* 
 * Author:    Thomas Baart
 * Contact:   mail@thomasbaart.nl
 * Date:      26-02-2014
 *
 * Tests for: ./wizard.js
 * Passes:    Yes, 26-02-2014
 */

describe('The wizard,', function () {
	var wizard;

	beforeEach(module('components.wizard'));

	beforeEach(inject(function (_wizard_) {
		wizard = _wizard_();
	}));

	afterEach(function () {
		wizard = null;
	});

	describe('post init,', function () {
		it('should not be null', function () {
			expect(wizard).not.toBe(null);
		});

		it('should have an index of 0', function () {
			expect(wizard.diagnostics().index).toBe(0);
		});

		it('should init with an array of objects', function () {
			var steps = [{ step: 'step 1' }, { step: 'step 2' }];
			wizard.init(steps);
			expect(wizard.current().step).toEqual(steps[0].step);
		});

		it('should throw an exception when it doesn\'t receive an array', function () {
			var steps = null;
			expect(wizard.init.bind(null, steps)).toThrow();
		});

		it('should throw an exception when any object supplied for steps does not contain a property "step"', function () {
			var steps = [{ step: 'step 1' }, { name: 'step 2' }];
			expect(wizard.init.bind(null, steps)).toThrow();
		});
	});

	describe('steps', function () {
	});

	describe('hasNext', function () {
		it('should have a next step when initialised with more than one step', function () {
			var steps = [{ step: 'step 1' }, { step: 'step 2' }];
			wizard.init(steps);
			expect(wizard.hasNext()).toBe(true);
		});

		it('should not have a next step when initialised with only one step', function () {
			var step = [{ step: 'step 1' }];
			wizard.init(step);
			expect(wizard.hasNext()).toBe(false);
		});
	});

	describe('next', function () {
		it('should be able to move to the next step when initialised with more than one step', function () {
			var steps = [{ step: 'step 1' }, { step: 'step 2' }];
			wizard.init(steps);
			wizard.next();
			expect(wizard.current().step).toEqual(steps[1].step);
		});

		it('should not be able to move to the next step when initialised with only one step', function () {
			var step = [{ step: 'step 1' }];
			wizard.init(step);
			wizard.next();
			expect(wizard.current().step).toEqual(step[0].step);
		});
	});

	describe('mayProceed', function () {
	});

	describe('hasPrev', function () {
		beforeEach(function () {
			var steps = [{ step: 'step 1' }, { step: 'step 2' }];
			wizard.init(steps);
		});

		it('should not have a prev step when initialised', function () {
			expect(wizard.hasPrev()).toBe(false);
		});

		it('should have a prev step when the index is higher than 0', function () {
			wizard.next();
			expect(wizard.hasPrev()).toBe(true);
		});
	});

	describe('prev', function () {
		beforeEach(function () {
			var steps = [{ step: 'step 1' }, { step: 'step 2' }];
			wizard.init(steps);
		});

		it('should not change the step when moving to prev after init', function () {
			wizard.prev();
			expect(wizard.diagnostics().index).toBe(0);
		});

		it('should change the step when moving to prev after next', function () {
			wizard.next();
			expect(wizard.diagnostics().index).toBe(1);
			wizard.prev();
			expect(wizard.diagnostics().index).toBe(0);
		});
	});

	describe('enable', function () {
		beforeEach(function () {
			var steps = [{ step: 'step 1' }, { step: 'step 2' }];
			wizard.init(steps);
		});

		it('should have enabled steps by default', function () {
			expect(wizard.current().enabled).toBe(true);
		});

		it('should be able to enable disabled steps', function () {
			wizard.disable(0);
			wizard.enable(0);
			expect(wizard.current().enabled).toBe(true);
		});
	});

	describe('disable', function () {
		beforeEach(function () {
			var steps = [{ step: 'step 1' }, { step: 'step 2' }];
			wizard.init(steps);
		});

		it('should be able to disable steps', function () {
			wizard.disable(0);
			expect(wizard.current().enabled).toBe(false);
		});
	});

	describe('step', function () {
		describe('visited', function () {
		});

		describe('isValid handler', function () {
		});
	});

	describe('handlers', function () {
		describe('onSubmit', function () {
		});

		describe('canSubmit', function () {
		});
	});
});