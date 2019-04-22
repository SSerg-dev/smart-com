Ext.define('App.controller.tpm.agegroup.HistoricalAgeGroup', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalagegroup directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalagegroup directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalagegroup #datatable': {
                    activate: this.onActivateCard
                },
                'historicalagegroup #detailform': {
                    activate: this.onActivateCard
                },
                'historicalagegroup #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalagegroup #table': {
                    click: this.onTableButtonClick
                },
                'historicalagegroup #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalagegroup #next': {
                    click: this.onNextButtonClick
                },
                'historicalagegroup #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalagegroup #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalagegroup #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
