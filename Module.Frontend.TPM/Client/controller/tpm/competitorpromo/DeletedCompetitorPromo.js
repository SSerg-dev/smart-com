Ext.define('App.controller.tpm.competitorpromo.DeletedCompetitorPromo', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedcompetitorpromo directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedcompetitorpromo directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedcompetitorpromo #datatable': {
                    activate: this.onActivateCard
                },
                'deletedcompetitorpromo #detailform': {
                    activate: this.onActivateCard
                },
                'deletedcompetitorpromo #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedcompetitorpromo #table': {
                    click: this.onTableButtonClick
                },
                'deletedcompetitorpromo #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedcompetitorpromo #next': {
                    click: this.onNextButtonClick
                },
                'deletedcompetitorpromo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedcompetitorpromo #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedcompetitorpromo #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedcompetitorpromo #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
