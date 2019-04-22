Ext.define('App.controller.tpm.retailtype.DeletedRetailType', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedretailtype directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedretailtype directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedretailtype #datatable': {
                    activate: this.onActivateCard
                },
                'deletedretailtype #detailform': {
                    activate: this.onActivateCard
                },
                'deletedretailtype #detail': {
                    click: this.switchToDetailForm
                },
                'deletedretailtype #table': {
                    click: this.onTableButtonClick
                },
                'deletedretailtype #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedretailtype #next': {
                    click: this.onNextButtonClick
                },
                'deletedretailtype #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedretailtype #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedretailtype #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedretailtype #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
