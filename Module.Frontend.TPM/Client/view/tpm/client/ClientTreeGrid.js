Ext.define('App.view.tpm.client.ClientTreeGrid', {
    extend: 'App.view.core.common.BaseTreeGrid',
    alias: 'widget.clienttreegrid',
    cls: 'template-tree scrollpanel',
    editorModel: 'Core.form.EditorWindowModel',
    blockChange: false,
    header: null,
    hideHeaders: true,

    lastScrollHeight: null,
    lastScrollY: null,

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
        // после создания/изменения узла необходимо загрузить дерево и сфокусироваться на этом узле
        var objectId = record.get('ObjectId');
        var me = this;

        if (objectId) {
            var store = tree.getStore();
            store.getProxy().extraParams.clientObjectId = objectId;
            store.getRootNode().removeAll();
            store.getRootNode().setId('root');
            store.load({
                scope: this,
                callback: function (records, operation, success) {
                    if (success) {
                        var choosenRecord = tree.getStore().getById(objectId);
                        if (choosenRecord.parentNode.isExpanded()) {
                            tree.getSelectionModel().select(choosenRecord);
                            tree.fireEvent('itemclick', tree.getView(), choosenRecord);

                            // если мы запомнили скролл, то можем к нему вернуться, при условии, если его высота не изменилась
                            var scroll = $('#vScrollClientTree' + tree.id).data('jsp');
                            if (scroll.getContentHeight() == me.lastScrollHeight) {
                                scroll.scrollToY(me.lastScrollY);
                            }
                            else {
                                var controllerClientTree = App.app.getController('tpm.client.ClientTree');
                                controllerClientTree.scrollNodeToCenterTree(tree, choosenRecord);
                            }
                        }
                    }

                    me.lastScrollHeight = null;
                    me.lastScrollY = null;
                }
            });
        }

        //var storeRecord = tree.getStore().getById(record.get('parentId')); // TODO: выделять сам элемент
        //if (storeRecord) {
        //    storeRecord.expand();
        //}
    },
    onDestroy: function () {
        var proxy = this.getStore().getProxy();
        proxy.extraParams.filterParameter = null; // TODO: переделать механизм фильтрации?
        proxy.extraParams.needBaseClients = true;
        this.callParent(arguments);
    },
});