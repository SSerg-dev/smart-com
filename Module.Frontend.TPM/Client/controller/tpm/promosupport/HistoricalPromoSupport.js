Ext.define('App.controller.tpm.promosupport.HistoricalPromoSupport', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalpromosupport directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalpromosupport directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalpromosupport #datatable': {
                    activate: this.onActivateCard
                },
                'historicalpromosupport #detailform': {
                    activate: this.onActivateCard
                },
                'historicalpromosupport #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalpromosupport #table': {
                    click: this.onTableButtonClick
                },
                'historicalpromosupport #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalpromosupport #next': {
                    click: this.onNextButtonClick
                },
                'historicalpromosupport #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalpromosupport #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalpromosupport #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
