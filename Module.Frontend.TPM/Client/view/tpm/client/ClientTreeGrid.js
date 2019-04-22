Ext.define('App.view.tpm.client.ClientTreeGrid', {
    extend: 'App.view.core.common.BaseTreeGrid',
    alias: 'widget.clienttreegrid',
    cls: 'template-tree scrollpanel',
    editorModel: 'Core.form.EditorWindowModel',
    blockChange: false,
    header: null,
    hideHeaders: true,
    viewConfig: {
        loadMask: false
    },
    columns: [
        {
            xtype: 'treecolumn',
            renderer: function (value, metaData, record, rowIdx, colIdx, store, view) {
                return record.isRoot() ? record.data.Name : record.data.Name + ' [' + record.data.Type + ']';
            },
            flex: 1
        },
    ],
    store: {
        storeId: 'clienttreestore',
        model: 'App.model.tpm.clienttree.ClientTree',
        autoLoad: false,
        root: {}
    },
    rootVisible: true,

    dockedItems: [],
    // разворачиваем узел в который добавили элемент
    afterSaveCallback: function (tree, record) {
        var storeRecord = tree.getStore().getById(record.get('parentId')); // TODO: выделять сам элемент
        if (storeRecord) {
            storeRecord.expand();
        }
    },
    onDestroy: function () {
        var proxy = this.getStore().getProxy();
        proxy.extraParams.filterParameter = null; // TODO: переделать механизм фильтрации?
        proxy.extraParams.needBaseClients = false;
        this.callParent(arguments);
    },
});