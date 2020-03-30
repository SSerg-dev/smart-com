Ext.define('App.controller.tpm.clientkpidata.HistoricalClientKPIData', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalclientkpidata directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalclientkpidata directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalclientkpidata #datatable': {
                    activate: this.onActivateCard
                },
                'historicalclientkpidata #detailform': {
                    activate: this.onActivateCard
                },
                'historicalclientkpidata #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalclientkpidata #table': {
                    click: this.onTableButtonClick
                },
                'historicalclientkpidata #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalclientkpidata #next': {
                    click: this.onNextButtonClick
                },
                'historicalclientkpidata #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalclientkpidata #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalclientkpidata #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
