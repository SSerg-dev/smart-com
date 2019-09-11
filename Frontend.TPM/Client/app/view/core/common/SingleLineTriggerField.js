Ext.define('App.view.core.common.SingleLineTriggerField', {
    extend: 'Ext.form.field.Trigger',
    alias: 'widget.singlelinetriggerfield',

    editable: false,

    onTriggerClick: function () {
        var win = Ext.widget(this.widgetName || 'viewtextdatawindow');
        var textField = win.down('field[name=content]');
        if (textField) {
            var value = this.getValue();
            textField.setValue(value);
            win.show();
        }
    },

    triggerCls: 'x-form-detail-trigger',

    fieldLabelTip: null,
    fieldValueTip: null,
    
    afterFirstLayout: function () {
        var me = this;
        this.callParent(arguments);

        if (me.labelEl) {
            me.fieldLabelTip = Ext.create('Ext.tip.ToolTip', {
                target: me.labelEl,
                trackMouse: true,
                html: me.getFieldLabel()
            });
        }

        if (me.inputEl) {
            me.fieldValueTip = Ext.create('Ext.tip.ToolTip', {
                target: me.inputEl,
                trackMouse: true,
                listeners: {
                    beforeshow: function (tip) {
                        tip.update(me.getValue());
                    }
                }
            });
        }
    },

    valueToRaw: function (value) {
        if (Ext.isBoolean(value)) {
            value = value.toString();
        }
        if (!value && value !== 0) {
            return '';
        } else {
            return value;
        }
    },
    
    fieldCls: 'x-form-display-field singlelinedisplayfield',

    labelClsExtra: 'singlelinedisplayfield',


});