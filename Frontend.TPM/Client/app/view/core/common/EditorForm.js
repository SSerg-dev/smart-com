Ext.define('App.view.core.common.EditorForm', {
    extend: 'Core.form.ColumnLayoutForm',
    alias: 'widget.editorform',

    ui: 'detailform-panel',
    cls: 'detailpanel',
    frame: true,
    ui: 'light-gray-panel',
    bodyPadding: '10 0 0 0',

    loadRecord: function (record) {
        this.form.loadRecord(record);
    },

    getRecord: function () {
        return this.form.getRecord();
    }
});