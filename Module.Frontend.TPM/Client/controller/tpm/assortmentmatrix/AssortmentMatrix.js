Ext.define('App.controller.tpm.assortmentmatrix.AssortmentMatrix', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'assortmentmatrix[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'assortmentmatrix': {
                    beforedestroy: this.onAssortmentMatrixBeforeDestroy,
                },
                'assortmentmatrix directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'assortmentmatrix #datatable': {
                    activate: this.onActivateCard
                },
                'assortmentmatrix #detailform': {
                    activate: this.onActivateCard
                },
                'assortmentmatrix #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'assortmentmatrix #detailform #next': {
                    click: this.onNextButtonClick
                },
                'assortmentmatrix #detail': {
                    click: this.onDetailButtonClick
                },
                'assortmentmatrix #table': {
                    click: this.onTableButtonClick
                },
                'assortmentmatrix #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'assortmentmatrix #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'assortmentmatrix #createbutton': {
                    click: this.onCreateButtonClick
                },
                'assortmentmatrix #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'assortmentmatrix #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'assortmentmatrix #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'assortmentmatrix #refresh': {
                    click: this.onRefreshButtonClick
                },
                'assortmentmatrix #close': {
                    click: this.onCloseButtonClick
                },
                'assortmentmatrix #actualassortmentmatrix': {
                    click: this.onActualAssortmentMatrixButtonClick
                },
                // import/export
                'assortmentmatrix #exportXlsxAssortmentMatrixButton': {
                    click: this.onExportButtonClick
                },
                'assortmentmatrix #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'assortmentmatrix #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'assortmentmatrix #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },
                'assortmentmatrixeditor': {
                    afterrender: this.afterrenderWindowEditor,
                },
                'assortmentmatrixeditor [name=ClientTreeId]': {
                    change: this.onClientTreeIdChange
                },
            }
        });
    },

    onGridAfterrender: function (grid) {
        //Click button "Get actual assortment matrix"
        var button = Ext.ComponentQuery.query('assortmentmatrix')[0].down('#actualassortmentmatrix');
        button.fireEvent('click', button);
    },

    afterrenderWindowEditor: function (window, eOpts) {
        var me = this;
        var assortmentmatrixeditor = Ext.ComponentQuery.query('assortmentmatrixeditor')[0];

        me.elements = {
            clientTreeId: assortmentmatrixeditor.down('[name=ClientTreeId]'),
            clientTreeObjectId: assortmentmatrixeditor.down('[name=ClientTreeObjectId]'),
        };
    },

    onClientTreeIdChange: function () {
        this.elements.clientTreeObjectId.setValue(this.elements.clientTreeId.getModelData().ClientTreeObjectId);
    },

    onCreateButtonClick: function () {
        this.callParent(arguments);

        var assortmentmatrixeditor = Ext.ComponentQuery.query('assortmentmatrixeditor')[0];
        var createDate = assortmentmatrixeditor.down('[name=CreateDate]');

        var date = new Date();
        date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);   // приведение к московской timezone
        createDate.setValue(date);                                              // вывести дату в поле 
    },

    onAssortmentMatrixBeforeDestroy: function (panel) {
        var assortmentMatrixGrid = panel.down('grid');
        var assortmentMatrixGridStore = assortmentMatrixGrid.getStore();
        var assortmentMatrixGridStoreProxy = assortmentMatrixGridStore.getProxy();
        assortmentMatrixGridStoreProxy.extraParams.needActualAssortmentMatrix = false;
        return true;
    },

    onActualAssortmentMatrixButtonClick: function (button) {
        var combinedDirectoryPanel = button.up('assortmentmatrix');
        var assortmentMatrixGrid = combinedDirectoryPanel.down('grid');
        var assortmentMatrixGridStore = assortmentMatrixGrid.getStore();
        var assortmentMatrixGridStoreProxy = assortmentMatrixGridStore.getProxy();

        assortmentMatrixGridStoreProxy.extraParams.needActualAssortmentMatrix = !assortmentMatrixGridStoreProxy.extraParams.needActualAssortmentMatrix;
        if (assortmentMatrixGridStoreProxy.extraParams.needActualAssortmentMatrix) {
            button.addCls('showEditablePromo-btn-active');
        } else {
            button.removeCls('showEditablePromo-btn-active');
        }
        
        assortmentMatrixGridStore.removeAll();
        assortmentMatrixGridStore.load();
    },

    onDeleteButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            panel = grid.up('combineddirectorypanel'),
            selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            Ext.Msg.show({
                title: l10n.ns('core').value('deleteWindowTitle'),
                msg: l10n.ns('core').value('deleteConfirmMessage'),
                fn: onMsgBoxClose,
                scope: this,
                icon: Ext.Msg.QUESTION,
                buttons: Ext.Msg.YESNO,
                buttonText: {
                    yes: l10n.ns('core', 'buttons').value('delete'),
                    no: l10n.ns('core', 'buttons').value('cancel')
                }
            });
        } else {
            console.log('No selection');
        }

        function onMsgBoxClose(buttonId) {

            if (buttonId === 'yes') {
                var record = selModel.getSelection()[0],
                    store = grid.getStore(),
                    view = grid.getView(),
                    currentIndex = store.indexOf(record),
                    pageIndex = store.getPageFromRecordIndex(currentIndex),
                    endIndex = store.getTotalCount() - 2; // 2, т.к. после удаления станет на одну запись меньше

                currentIndex = Math.min(Math.max(currentIndex, 0), endIndex);
                panel.setLoading(l10n.ns('core').value('deletingText'));

                record.destroy({
                    scope: this,
                    success: function () {
                        selModel.deselectAll();
                        store.data.removeAtKey(pageIndex);
                        store.totalCount--;

                        grid.setLoadMaskDisabled(true);
                        store.on({
                            single: true,
                            load: function (records, operation, success) {
                                if (store.getTotalCount() > 0) {
                                    view.bufferedRenderer.scrollTo(currentIndex, true, function () {
                                        view.refresh();
                                        view.focusRow(currentIndex);
                                    });
                                }

                                panel.setLoading(false);
                                grid.setLoadMaskDisabled(false);
                            }
                        });
                        store.load();
                    },
                    failure: function () {
                        panel.setLoading(false);
                    }
                });
            }
        }
    },

    onExportButtonClick: function (button) {
        debugger
        var me = this;
        var grid = me.getGridByButton(button);
        var panel = grid.up('combineddirectorypanel');
        var store = grid.getStore();
        var proxy = store.getProxy();
        var actionName = button.action || 'ExportXLSX';
        var resource = button.resource || proxy.resourceName;
        panel.setLoading(true);
        var query = breeze.EntityQuery
            .from(resource)
            .withParameters({
                $actionName: actionName,
                $method: 'POST',
                needActualAssortmentMatrix: proxy.extraParams.needActualAssortmentMatrix//if button "Get actual assortment matrix" is pressed or not
            });

        query = me.buildQuery(query, store)
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .execute()
            .then(function (data) {
                panel.setLoading(false);
                App.Notify.pushInfo('Export task created successfully');
                App.System.openUserTasksPanel()
            })
            .fail(function (data) {
                panel.setLoading(false);
                App.Notify.pushError(me.getErrorMessage(data));
            });
    }
});
