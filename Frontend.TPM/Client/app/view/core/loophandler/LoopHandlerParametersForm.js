Ext.define('App.view.core.loophandler.LoopHandlerParametersForm', {
    extend: 'Ext.form.Panel',
    xtype: 'loophandlerparametersform',
    ui: 'detailform-panel',

    bodyPadding: '10 10 0 10',

    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'center'
    },

    defaults: {
        xtype: 'singlelinedisplayfield',
        ui: 'detail-form-field',
        labelAlign: 'left',
        labelWidth: 200,
        labelSeparator: '',
        labelPad: 0
    }
});