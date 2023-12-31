﻿Ext.define('App.controller.tpm.promoroireport.PromoROIReport', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promoroireport[isMain=true][isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                },
                'promoroireport[isSearch!=true] directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                },
                'promoroireport directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridPromoROIReportAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promoroireport #datatable': {
                    activate: this.onActivateCard
                },
                'promoroireport #detailform': {
                    activate: this.onActivateCard
                },
                'promoroireport #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'promoroireport #detailform #next': {
                    click: this.onNextButtonClick
                },
                'promoroireport #detail': {
                    click: this.onDetailButtonClick
                },
                'promoroireport #table': {
                    click: this.onTableButtonClick
                },
                'promoroireport #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promoroireport #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promoroireport #createbutton': {
                    click: this.onCreateButtonClick
                },
                'promoroireport #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promoroireport #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promoroireport #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promoroireport #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promoroireport #close': {
                    click: this.onCloseButtonClick
                },
                'promoroireport #exportbutton': {
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
        }
        else {
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