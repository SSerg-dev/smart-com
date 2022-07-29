Ext.define('App.controller.tpm.rsmode.RSmode', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'rsmode[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'rsmode directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'rsmode #datatable': {
                    activate: this.onActivateCard
                },
                'rsmode #detailform': {
                    activate: this.onActivateCard
                },
                'rsmode #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'rsmode #detailform #next': {
                    click: this.onNextButtonClick
                },
                'rsmode #detail': {
                    click: this.switchToDetailForm
                },
                'rsmode #table': {
                    click: this.onTableButtonClick
                },
                'rsmode #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'rsmode #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'rsmode #createbutton': {
                    click: this.onCreateButtonClick
                },
                'rsmode #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'rsmode #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'rsmode #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'rsmode #refresh': {
                    click: this.onRefreshButtonClick
                },
                'rsmode #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'rsmode #exportbutton': {
                    click: this.onExportButtonClick
                },
                'rsmode #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'rsmode #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'rsmode #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },
    getRSPeriod: function (cb_func) {

        $.ajax({
            dataType: 'json',
            url: '/odata/Promoes/GetRSPeriod',
            type: 'POST',
            success: function (response) {
                var data = Ext.JSON.decode(response.value);
                if (data.success) {
                    cb_func(data.startEndModel);
                }
                else {
                    App.Notify.pushError(l10n.ns('tpm', 'text').value('failedStatusLoad'));
                }
            },
            error: function (data) {
                App.Notify.pushError(data.responseJSON["odata.error"].innererror.message);
            }
        });
    },
});
