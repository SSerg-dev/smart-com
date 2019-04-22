Ext.define('App.controller.tpm.category.HistoricalCategory', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalcategory directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalcategory directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalcategory #datatable': {
                    activate: this.onActivateCard
                },
                'historicalcategory #detailform': {
                    activate: this.onActivateCard
                },
                'historicalcategory #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalcategory #table': {
                    click: this.onTableButtonClick
                },
                'historicalcategory #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalcategory #next': {
                    click: this.onNextButtonClick
                },
                'historicalcategory #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalcategory #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalcategory #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
