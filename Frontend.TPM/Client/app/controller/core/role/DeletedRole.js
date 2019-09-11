Ext.define('App.controller.core.role.DeletedRole', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedrole directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedrole directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'deletedrole #datatable': {
                    activate: this.onActivateCard
                },
                'deletedrole #detailform': {
                    activate: this.onActivateCard
                },
                'deletedrole #detail': {
                    click: this.switchToDetailForm
                },
                'deletedrole #table': {
                    click: this.onTableButtonClick
                },
                'deletedrole #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedrole #next': {
                    click: this.onNextButtonClick
                },
                //

                'deletedrole #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedrole #historybutton': {
                    click: this.onHistoryButtonClick
                },

                'deletedrole #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedrole #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});