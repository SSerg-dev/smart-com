Ext.define('App.view.core.common.SingleLineDisplayField', {
    extend: 'Ext.form.field.Display',
    alias: 'widget.singlelinedisplayfield',

    labelClsExtra: 'singleline-lable',
    fieldCls: 'x-form-display-field singlelinedisplayfield',

    valueToRaw: function (value) {
        if (Ext.isBoolean(value)) {
            value = value.toString();
        }
        return this.callParent([value]);
    }

});