Ext.define('App.controller.tpm.nonpromoequipment.DeletedNonPromoEquipment', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletednonpromoequipment directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletednonpromoequipment directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletednonpromoequipment #datatable': {
                    activate: this.onActivateCard
                },
                'deletednonpromoequipment #detailform': {
                    activate: this.onActivateCard
                },
                'deletednonpromoequipment #detail': {
                    click: this.onDetailButtonClick
                },
                'deletednonpromoequipment #table': {
                    click: this.onTableButtonClick
                },
                'deletednonpromoequipment #prev': {
                    click: this.onPrevButtonClick
                },
                'deletednonpromoequipment #next': {
                    click: this.onNextButtonClick
                },
                'deletednonpromoequipment #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletednonpromoequipment #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletednonpromoequipment #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletednonpromoequipment #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
