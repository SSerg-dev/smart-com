Ext.define('App.controller.tpm.promosupport.PromoSupportChoose', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promosupportchoose[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'promosupportchoose directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promosupportchoose #datatable': {
                    activate: this.onActivateCard
                },
                'promosupportchoose #detailform': {
                    activate: this.onActivateCard
                },
                'promosupportchoose #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'promosupportchoose #detailform #next': {
                    click: this.onNextButtonClick
                },
                'promosupportchoose #detail': {
                    click: this.onDetailButtonClick
                },
                'promosupportchoose #table': {
                    click: this.onTableButtonClick
                },
                'promosupportchoose #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promosupportchoose #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promosupportchoose #createbutton': {
                    click: this.onCreateButtonClick
                },
                'promosupportchoose #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promosupportchoose #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promosupportchoose #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promosupportchoose #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promosupportchoose #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'promosupportchoose #exportbutton': {
                    click: this.onExportButtonClick
                },
                'promosupportchoose #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'promosupportchoose #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'promosupportchoose #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },
                'promosupportchoose gridcolumn[cls=select-all-header]': {
                    headerclick: this.onSelectAllRecordsClick,
                    afterrender: this.clearBaseSelectAllRecordsHandler,
                }
            }
        });
    },

    clearBaseSelectAllRecordsHandler: function (header) {
        // избавляемся от некорректного обработчика
        var headerCt = header.up('headercontainer');

        if (headerCt.events.headerclick.listeners.length == 2) {
            headerCt.events.headerclick.listeners.pop();
        }
    },

    onSelectAllRecordsClick: function (headerCt, header) {
        var grid = header.up('directorygrid'),
            win = headerCt.up('selectorwindow'),
            store = grid.getStore(),
            selModel = grid.getSelectionModel(),
            recordsCount = store.getTotalCount(),
            functionChecker = selModel.checkedRows.length == recordsCount ? selModel.uncheckRows : selModel.checkRows;

        if (recordsCount > 0) {
            grid.setLoading(true);
            store.getRange(0, recordsCount, {
                callback: function () {
                    if (recordsCount > 0) {
                        functionChecker.call(selModel, store.getRange(0, recordsCount));
                        grid.setLoading(false);
                    }
                    if (win) {
                        win.down('#select')[selModel.hasChecked() ? 'enable' : 'disable']();
                    }
                }
            });
        }

        grid.fireEvent('selectionchange', selModel);
    }
});