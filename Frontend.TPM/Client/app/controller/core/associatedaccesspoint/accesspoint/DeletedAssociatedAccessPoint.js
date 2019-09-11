Ext.define('App.controller.core.associatedaccesspoint.accesspoint.DeletedAssociatedAccessPoint', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedassociatedaccesspointaccesspoint directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedassociatedaccesspointaccesspoint directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'deletedassociatedaccesspointaccesspoint #datatable': {
                    activate: this.onActivateCard
                },
                'deletedassociatedaccesspointaccesspoint #detailform': {
                    activate: this.onActivateCard
                },
                'deletedassociatedaccesspointaccesspoint #detail': {
                    click: this.switchToDetailForm
                },
                'deletedassociatedaccesspointaccesspoint #table': {
                    click: this.onTableButtonClick
                },
                'deletedassociatedaccesspointaccesspoint #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedassociatedaccesspointaccesspoint #next': {
                    click: this.onNextButtonClick
                },
                //

                'deletedassociatedaccesspointaccesspoint #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedassociatedaccesspointaccesspoint #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedassociatedaccesspointaccesspoint #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedassociatedaccesspointaccesspoint #close': {
                    click: this.onCloseButtonClick
                }

            }
        });
    }

});