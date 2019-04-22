Ext.define('App.controller.tpm.commercialnet.DeletedCommercialNet', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedcommercialnet directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedcommercialnet directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedcommercialnet #datatable': {
                    activate: this.onActivateCard
                },
                'deletedcommercialnet #detailform': {
                    activate: this.onActivateCard
                },
                'deletedcommercialnet #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedcommercialnet #table': {
                    click: this.onTableButtonClick
                },
                'deletedcommercialnet #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedcommercialnet #next': {
                    click: this.onNextButtonClick
                },
                'deletedcommercialnet #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedcommercialnet #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedcommercialnet #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedcommercialnet #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
