Ext.define('App.controller.tpm.technology.HistoricalTechnology', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicaltechnology directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicaltechnology directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicaltechnology #datatable': {
                    activate: this.onActivateCard
                },
                'historicaltechnology #detailform': {
                    activate: this.onActivateCard
                },
                'historicaltechnology #detail': {
                    click: this.onDetailButtonClick
                },
                'historicaltechnology #table': {
                    click: this.onTableButtonClick
                },
                'historicaltechnology #prev': {
                    click: this.onPrevButtonClick
                },
                'historicaltechnology #next': {
                    click: this.onNextButtonClick
                },
                'historicaltechnology #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicaltechnology #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicaltechnology #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
