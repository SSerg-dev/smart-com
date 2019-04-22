Ext.define('App.controller.tpm.postpromoeffect.DeletedPostPromoEffect', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedpostpromoeffect directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedpostpromoeffect directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedpostpromoeffect #datatable': {
                    activate: this.onActivateCard
                },
                'deletedpostpromoeffect #detailform': {
                    activate: this.onActivateCard
                },
                'deletedpostpromoeffect #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedpostpromoeffect #table': {
                    click: this.onTableButtonClick
                },
                'deletedpostpromoeffect #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedpostpromoeffect #next': {
                    click: this.onNextButtonClick
                },
                'deletedpostpromoeffect #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedpostpromoeffect #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedpostpromoeffect #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedpostpromoeffect #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
