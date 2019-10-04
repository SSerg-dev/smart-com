Ext.define('App.controller.tpm.budgetsubitem.BudgetSubItemClientTree', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'budgetsubitemclienttree directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'budgetsubitemclienttree #datatable': {
                    activate: this.onActivateCard
                },
                'budgetsubitemclienttree #detailform': {
                    activate: this.onActivateCard
                },
                'budgetsubitemclienttree #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'budgetsubitemclienttree #detailform #next': {
                    click: this.onNextButtonClick
                },
                'budgetsubitemclienttree #detail': {
                    click: this.onDetailButtonClick
                },
                'budgetsubitemclienttree #table': {
                    click: this.onTableButtonClick
                },
                'budgetsubitemclienttree #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'budgetsubitemclienttree #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'budgetsubitemclienttree #addbutton': {
                    click: this.onAddButtonClick
                },
                '#associatedbudgetsubitemclienttree_clienttree_selectorwindow basetreegrid': {
                    checkchange: this.onCheckChange,
                },
                '#associatedbudgetsubitemclienttree_clienttree_selectorwindow #select': {
                    click: this.onSelectButtonClick
                },

                'budgetsubitemclienttree #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'budgetsubitemclienttree #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'budgetsubitemclienttree #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'budgetsubitemclienttree #refresh': {
                    click: this.onRefreshButtonClick
                },
                'budgetsubitemclienttree #close': {
                    click: this.onCloseButtonClick
                },

                'clienttree #selectAllClientsCheckbox': {
                    change: this.onSelectAllChecked
                }
            }
        });
    },

    onAddButtonClick: function (button) {
        var cdPanel = this.getSelectorPanel(),
            me = this;

        if (cdPanel) {
            var widget = Ext.widget('selectorwindow', {
                            title: l10n.ns('core').value('selectorWindowTitle'),
                            itemId: Ext.String.format('{0}_{1}_{2}', button.up('associateddirectoryview').getXType(), cdPanel.getXType(), 'selectorwindow'),
                            ownerGrid: button.up('combineddirectorypanel').down('directorygrid'),
                            items: [cdPanel]
                        });//.show();

            var clientTreeStore = widget.down('clienttreegrid').getStore(),
                clientTreeProxy = clientTreeStore.getProxy(),
                budgetSubItemGrid = button.up('associatedbudgetsubitemclienttree').down('budgetsubitem').down('directorygrid'),
                selModel = budgetSubItemGrid.getSelectionModel();

            if (selModel.hasSelection()) {
                var selected = selModel.getSelection()[0];
                clientTreeProxy.extraParams.budgetSubItemId = selected.data.Id;
                widget.budgetSubItemId = selected.data.Id;
            }

            clientTreeStore.on('load', function (store, node, records, successful) {
                // выделяем/развыделяем все, кроме корневого узла
                if (records && records.length > 0 && records[0].childNodes) {
                    me.checkNodes(records[0].childNodes);
                }
            });

            widget.show();
            clientTreeProxy.extraParams.budgetSubItemId = null;
        }
    },

    getSelectorPanel: function () {
        var clienttree = Ext.widget('clienttree');
        clienttree.hideNotHierarchyBtns = true;
        clienttree.chooseMode = true;
        return clienttree;
    },

    onCheckChange: function (node, checked) {
        var treegrid = node.getOwnerTree(),
            clientTree = treegrid.up('clienttree'),
            clientTreeGrid = clientTree.down('basetreegrid'),
            countChecked = clientTreeGrid.getChecked();

        var btn = Ext.ComponentQuery.query('#associatedbudgetsubitemclienttree_clienttree_selectorwindow #select')[0];
        btn[countChecked.length > 0 ? 'enable' : 'disable']();
    },

    onSelectButtonClick: function (button) {
        var clientTree = button.up('#associatedbudgetsubitemclienttree_clienttree_selectorwindow'),
            clientTreeGrid = clientTree.down('basetreegrid'),
            checkedNodes = clientTreeGrid.getChecked(),
            checkedIds = new String();

        if (checkedNodes) {
            checkedNodes.forEach(function (item) {
                checkedIds = checkedIds.concat(item.data.Id.toString() + ';');
            });
        }
        //Удаляем последнюю запятую, что бы работал парсер на беке
        checkedIds = checkedIds.slice(0, -1);

        var window = button.up('window'),
            ownerGrid = window.ownerGrid,
            parentPanel = ownerGrid.up('combineddirectorypanel').getParent();
        selectedSubItemId = parentPanel.down('directorygrid').getSelectionModel().getSelection()[0].getId();

        window.setLoading(l10n.ns('core').value('savingText'));
        $.ajax({
            type: "POST",
            cache: false,
            url: "/odata/BudgetSubItemClientTrees?selectedSubItemId=" + selectedSubItemId,
            data: checkedIds,
            dataType: "json",
            contentType: false,
            processData: false,
            success: function (response) {
                var result = Ext.JSON.decode(response.value);
                if (result.success) {
                    window.destroy();
                    ownerGrid.getStore().load();
                    window.setLoading(false);
                }
                else {
                    App.Notify.pushError(result.message);
                    window.setLoading(false);
                }
            }
        });
    },

    onSelectAllChecked: function (checkbox, newValue, oldValue) {
        var clientTree = checkbox.up('clienttree'),
            window = clientTree.up('window'),
            btn = window.down('#select'),
            textFieldSearch = clientTree.down('#clientsSearchTrigger'),
            baseClientChBox = clientTree.down('#baseClientsCheckbox'),
            store = clientTree.down('basetreegrid').store,
            proxy = store.getProxy(),
            me = this;

        var textSearch = textFieldSearch.getValue()
        if (textSearch && textSearch.length > 0 && textSearch.indexOf('Client search') == -1) {
            proxy.extraParams.filterParameter = textSearch;
        } else if (newValue) {
            proxy.extraParams.filterParameter = 'Client';
        }

        proxy.extraParams.needBaseClients = baseClientChBox.getValue();

        if (clientTree.choosenClientObjectId)
            proxy.extraParams.clientObjectId = clientTree.choosenClientObjectId;

        proxy.extraParams.selectAllClients = newValue;

        store.getRootNode().removeAll();
        store.getRootNode().setId('root');
        store.load();

        store.on('load', function (store, node, records, successful) {
            // выделяем/развыделяем все, кроме корневого узла
            if (records && records.length > 0 && records[0].childNodes) {
                me.checkNodes(records[0].childNodes, newValue);
                btn[newValue ? 'enable' : 'disable']();
            }
        });

        proxy.extraParams.filterParameter = null;
        proxy.extraParams.needBaseClients = false;
        proxy.extraParams.clientObjectId = null;
        proxy.extraParams.selectAllClients = false;
    },

    checkNodes: function (nodes, checked) {
        var me = this;
        if (nodes && nodes.length > 0) {
            nodes.forEach(function (node, index, array) {
                node.set('checked', checked || node.get('_checked'));
                me.checkNodes(node.childNodes, checked);
            })
        }
    }
});