Ext.define('App.view.tpm.pludictionary.PLUDictionaryEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.pludictionaryeditor',
    width: 800,
    minWidth: 800,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 2,
        items: []
    }
});