Ext.define('App.controller.tpm.clienttreebrandtech.DeletedClientTreeBrandTech', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedclienttreebrandtech directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedclienttreebrandtech directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedclienttreebrandtech #datatable': {
                    activate: this.onActivateCard
                },
                'deletedclienttreebrandtech #detailform': {
                    activate: this.onActivateCard
                },
                'deletedclienttreebrandtech #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedclienttreebrandtech #table': {
                    click: this.onTableButtonClick
                },
                'deletedclienttreebrandtech #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedclienttreebrandtech #next': {
                    click: this.onNextButtonClick
                },
                'deletedclienttreebrandtech #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedclienttreebrandtech #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedclienttreebrandtech #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedclienttreebrandtech #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
