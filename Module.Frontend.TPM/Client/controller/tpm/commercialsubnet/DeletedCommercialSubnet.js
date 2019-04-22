Ext.define('App.controller.tpm.commercialsubnet.DeletedCommercialSubnet', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedcommercialsubnet directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedcommercialsubnet directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedcommercialsubnet #datatable': {
                    activate: this.onActivateCard
                },
                'deletedcommercialsubnet #detailform': {
                    activate: this.onActivateCard
                },
                'deletedcommercialsubnet #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedcommercialsubnet #table': {
                    click: this.onTableButtonClick
                },
                'deletedcommercialsubnet #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedcommercialsubnet #next': {
                    click: this.onNextButtonClick
                },
                'deletedcommercialsubnet #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedcommercialsubnet #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedcommercialsubnet #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedcommercialsubnet #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
