Ext.define('App.controller.tpm.promopriceincreaseroireport.PromoPriceIncreaseROIReport', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promopriceincreaseroireport[isMain=true][isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                },
                'promopriceincreaseroireport[isSearch!=true] directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                },
                'promopriceincreaseroireport directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridPromoROIReportAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promopriceincreaseroireport #datatable': {
                    activate: this.onActivateCard
                },
                'promopriceincreaseroireport #detailform': {
                    activate: this.onActivateCard
                },
                'promopriceincreaseroireport #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'promopriceincreaseroireport #detailform #next': {
                    click: this.onNextButtonClick
                },
                'promopriceincreaseroireport #detail': {
                    click: this.onDetailButtonClick
                },
                'promopriceincreaseroireport #table': {
                    click: this.onTableButtonClick
                },
                'promopriceincreaseroireport #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promopriceincreaseroireport #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promopriceincreaseroireport #createbutton': {
                    click: this.onCreateButtonClick
                },
                'promopriceincreaseroireport #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promopriceincreaseroireport #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promopriceincreaseroireport #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promopriceincreaseroireport #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promopriceincreaseroireport #close': {
                    click: this.onCloseButtonClick
                },
                'promopriceincreaseroireport #exportbutton': {
                    click: this.onExportROIReportButtonClick
                },
                
            }
        });
    },

    onGridPromoROIReportAfterrender: function (grid) {
        thisGrid = grid;
        var RSmodeController = App.app.getController('tpm.rsmode.RSmode');
        if (!TpmModes.isRsRaMode()) {
            var indexh = this.getColumnIndex(grid, 'TPMmode');
            grid.columnManager.getColumns()[indexh].hide();
        } else {
            var promoROIReportGridStore = grid.getStore();
            var promoROIReportGridStoreProxy = promoROIReportGridStore.getProxy();
            promoROIReportGridStoreProxy.extraParams.TPMmode = TpmModes.getSelectedMode().alias;
        }
        this.onGridAfterrender(grid);
    },

    getColumnIndex: function (grid, dataIndex) {
        gridColumns = grid.headerCt.getGridColumns();
        for (var i = 0; i < gridColumns.length; i++) {
            if (gridColumns[i].dataIndex == dataIndex) {
                return i;
            }
        }
    },

    onExportROIReportButtonClick: function (button) {
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
                tPMmode: TpmModes.getSelectedModeId()
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

    },
});