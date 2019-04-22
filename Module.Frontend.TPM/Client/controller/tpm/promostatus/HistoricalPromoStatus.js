Ext.define('App.controller.tpm.promostatus.HistoricalPromoStatus', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalpromostatus directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalpromostatus directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalpromostatus #datatable': {
                    activate: this.onActivateCard
                },
                'historicalpromostatus #detailform': {
                    activate: this.onActivateCard
                },
                'historicalpromostatus #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalpromostatus #table': {
                    click: this.onTableButtonClick
                },
                'historicalpromostatus #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalpromostatus #next': {
                    click: this.onNextButtonClick
                },
                'historicalpromostatus #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalpromostatus #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalpromostatus #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
