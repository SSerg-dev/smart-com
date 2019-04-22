Ext.define('App.controller.tpm.storetype.DeletedStoreType', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedstoretype directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedstoretype directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedstoretype #datatable': {
                    activate: this.onActivateCard
                },
                'deletedstoretype #detailform': {
                    activate: this.onActivateCard
                },
                'deletedstoretype #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedstoretype #table': {
                    click: this.onTableButtonClick
                },
                'deletedstoretype #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedstoretype #next': {
                    click: this.onNextButtonClick
                },
                'deletedstoretype #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedstoretype #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedstoretype #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedstoretype #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
