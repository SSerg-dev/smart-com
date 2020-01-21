Ext.define('App.controller.tpm.promo.HistoricalUserRolePromo', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicaluserrolepromo directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicaluserrolepromo directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicaluserrolepromo #datatable': {
                    activate: this.onActivateCard
                },
                'historicaluserrolepromo #detailform': {
                    activate: this.onActivateCard
                },
                'historicaluserrolepromo #detail': {
                    click: this.onDetailButtonClick
                },
                'historicaluserrolepromo #table': {
                    click: this.onTableButtonClick
                },
                'historicaluserrolepromo #prev': {
                    click: this.onPrevButtonClick
                },
                'historicaluserrolepromo #next': {
                    click: this.onNextButtonClick
                },
                'historicaluserrolepromo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicaluserrolepromo #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicaluserrolepromo #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
