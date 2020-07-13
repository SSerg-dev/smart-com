Ext.define('App.controller.tpm.clienttreebrandtech.HistoricalClientTreeBrandTech', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicalclienttreebrandtech directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicalclienttreebrandtech directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicalclienttreebrandtech #datatable': {
                    activate: this.onActivateCard
                },
                'historicalclienttreebrandtech #detailform': {
                    activate: this.onActivateCard
                },
                'historicalclienttreebrandtech #detail': {
                    click: this.onDetailButtonClick
                },
                'historicalclienttreebrandtech #table': {
                    click: this.onTableButtonClick
                },
                'historicalclienttreebrandtech #prev': {
                    click: this.onPrevButtonClick
                },
                'historicalclienttreebrandtech #next': {
                    click: this.onNextButtonClick
                },
                'historicalclienttreebrandtech #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicalclienttreebrandtech #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicalclienttreebrandtech #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
