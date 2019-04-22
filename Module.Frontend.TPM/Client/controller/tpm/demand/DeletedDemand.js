Ext.define('App.controller.tpm.demand.DeletedDemand', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deleteddemand directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deleteddemand directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deleteddemand #datatable': {
                    activate: this.onActivateCard
                },
                'deleteddemand #detailform': {
                    activate: this.onActivateCard
                },
                'deleteddemand #detail': {
                    click: this.onDetailButtonClick
                },
                'deleteddemand #table': {
                    click: this.onTableButtonClick
                },
                'deleteddemand #prev': {
                    click: this.onPrevButtonClick
                },
                'deleteddemand #next': {
                    click: this.onNextButtonClick
                },
                'deleteddemand #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deleteddemand #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deleteddemand #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deleteddemand #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
