Ext.define('Ext.ux.form.field.FieldClearButton', {
    extend: 'Ext.AbstractPlugin',
    alias: 'plugin.fieldclearbutton',
    pluginId: 'fieldclearbutton',

    mixins: {
        observable: 'Ext.util.Observable'
    },

    hideClearButtonWhenEmpty: true,

    clearBtnCls: 'ux-fieldclearbutton',
    clearBtnInnerCls: Ext.baseCSSPrefix + 'form-clear-trigger',

    isTriggerField: false,
    formField: null,
    clearBtnEl: null,
    clearBtnInnerEl: null,

    constructor: function (cfg) {
        Ext.apply(this, cfg);
        this.callParent(arguments);
        this.mixins.observable.constructor.call(this, cfg);
        this.addEvents('clear');
    },

    init: function (formField) {
        this.formField = formField;
        if (!formField.rendered) {
            formField.on('afterrender', this.handleAfterRender, this);
        }
        else {
            this.handleAfterRender();
        }
    },

    handleAfterRender: function (formField) {
        this.formField = formField;

        if (formField.triggerWrap) {
            var triggerWrapEl = formField.triggerWrap;
            triggerWrapEl.addCls(this.clearBtnCls);
            this.clearBtnEl = Ext.get(triggerWrapEl.dom.tBodies[0].rows[0].insertCell(-1));
            this.isTriggerField = true;
        } else {
            var el = formField.getEl();
            el.addCls(this.clearBtnCls);
            el.addCls('x-form-trigger-wrap');
            this.clearBtnEl = Ext.get(el.dom.tBodies[0].rows[0].insertCell(-1));
        }

        if (this.clearBtnEl) {
            this.addListeners();
        }
    },

    addListeners: function () {
        this.formField.on('change', this.updateClearButtonVisibility, this);
        this.formField.on('focus', this.onFormFieldFocus, this);
        this.formField.on('blur', this.onFormFieldBlur, this);
        this.clearBtnEl.on('click', this.onClearButtonClick, this);
    },

    updateClearButtonVisibility: function () {
        var oldVisible = this.isButtonCurrentlyVisible(),
            newVisible = this.shouldButtonBeVisible(),
            clearBtnEl = this.clearBtnEl;

        if (oldVisible != newVisible) {
            if (newVisible && !this.clearBtnInnerEl) {
                this.clearBtnInnerEl = clearBtnEl.createChild({
                    tag: 'div',
                    cls: this.clearBtnInnerCls
                });
            }
            clearBtnEl.setDisplayed(newVisible);
        }
    },

    onFormFieldFocus: function () {
        if (!this.isTriggerField) {
            this.formField.getEl().up('div').addCls('x-form-trigger-wrap-focus');
        }
    },

    onFormFieldBlur: function () {
        if (!this.isTriggerField) {
            this.formField.getEl().up('div').removeCls('x-form-trigger-wrap-focus');
        }
    },

    onClearButtonClick: function () {
        if (this.formField.clearValue) {
            this.formField.clearValue();
        } else {
            this.formField.setValue('');
        }

        this.fireEvent('clear', this.formField);
        this.formField.focus();
    },

    isButtonCurrentlyVisible: function () {
        return this.clearBtnInnerEl && this.clearBtnEl.isDisplayed();
    },

    shouldButtonBeVisible: function () {
        return !(this.hideClearButtonWhenEmpty && Ext.isEmpty(this.formField.getValue()));
    }
});
