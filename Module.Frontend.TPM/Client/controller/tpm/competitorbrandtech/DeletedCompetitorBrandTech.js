Ext.define('App.controller.tpm.competitorbrandtech.DeletedCompetitorBrandTech', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedcompetitorbrandtech directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedcompetitorbrandtech directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedcompetitorbrandtech #datatable': {
                    activate: this.onActivateCard
                },
                'deletedcompetitorbrandtech #detailform': {
                    activate: this.onActivateCard
                },
                'deletedcompetitorbrandtech #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedcompetitorbrandtech #table': {
                    click: this.onTableButtonClick
                },
                'deletedcompetitorbrandtech #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedcompetitorbrandtech #next': {
                    click: this.onNextButtonClick
                },
                'deletedcompetitorbrandtech #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedcompetitorbrandtech #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedcompetitorbrandtech #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedcompetitorbrandtech #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
