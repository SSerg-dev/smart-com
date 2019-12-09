Ext.define('App.util.TPM.TextFieldWithTrigger', {
    extend: 'Ext.form.field.Trigger',
    alias: 'widget.textfieldwithtrigger',

    trigger1Cls: Ext.baseCSSPrefix + 'form-clear-trigger',
    fillerText: '',

    onRender: function () {
        var me = this,
            el;
        me.callParent(arguments);
        me.setValue(me.fillerText);
        me.addCls('fillerText');
        el = me.triggerEl.first().parent();
        el.setVisibilityMode(Ext.Element.DISPLAY);
        me.on('focus', me.onFocusCustom);
        me.on('blur', me.onBlurCustom);
        el.hide();
    },

    onFocusCustom: function (me) {
        if (me.value === me.fillerText) {
            me.setValue('');
        }
    },

    onBlurCustom: function (me) {
        if (!me.value) {
            me.setValue(me.fillerText);
            me.addCls('fillerText');
        }
    },

    onChange: function (newVal, oldVal) {
        var me = this;
        me.removeCls('fillerText');
        el = me.triggerEl.first().parent();
        me.callParent(arguments);
        if (newVal.length) {
            if (newVal != me.fillerText) {
                el.show();
            }
        } else {
            el.hide();
        }
    },

    onTrigger1Click: function (event) {
        var me = this;
        if (me.hideTrigger) {
            return;
        }
        me.setValue('');
        me.fireEvent('cleartriggerclick', me, event);
    }
});
