Ext.define('Core.form.BaseEditorModel', {
    config: {
        grid: null,
        tree: null
    },

    constructor: function (config) {
        this.callParent(arguments);
        this.initConfig(config);
    },

    startCreateRecord: function (model) {},

    startEditRecord: function (model) { },

    onOkButtonClick: function (button) { },

    onCancelButtonClick: function (button) { },

    getForm: function () { },

    saveModel: function (model, callback) { }
})