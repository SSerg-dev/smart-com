Ext.define('App.controller.tpm.client.DeletedClient', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedclient directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedclient directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedclient #datatable': {
                    activate: this.onActivateCard
                },
                'deletedclient #detailform': {
                    activate: this.onActivateCard
                },
                'deletedclient #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedclient #table': {
                    click: this.onTableButtonClick
                },
                'deletedclient #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedclient #next': {
                    click: this.onNextButtonClick
                },
                'deletedclient #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedclient #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedclient #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedclient #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
