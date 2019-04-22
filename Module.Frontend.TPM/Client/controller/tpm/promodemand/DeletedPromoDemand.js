Ext.define('App.controller.tpm.promodemand.DeletedPromoDemand', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedpromodemand directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedpromodemand directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedpromodemand #datatable': {
                    activate: this.onActivateCard
                },
                'deletedpromodemand #detailform': {
                    activate: this.onActivateCard
                },
                'deletedpromodemand #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedpromodemand #table': {
                    click: this.onTableButtonClick
                },
                'deletedpromodemand #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedpromodemand #next': {
                    click: this.onNextButtonClick
                },
                'deletedpromodemand #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedpromodemand #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedpromodemand #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedpromodemand #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
