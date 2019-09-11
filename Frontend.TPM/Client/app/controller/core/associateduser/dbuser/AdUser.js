Ext.define('App.controller.core.associateduser.user.AdUser', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'aduser[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad
                },
                'aduser directorygrid': {
                    // TODO: относиться к переключению состояний грида, можно убрать в будущем
                    itemdblclick: this.switchToDetailForm,

                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },

                // TODO: относиться к переключению состояний грида, можно убрать в будущем
                'aduser #datatable': {
                    activate: this.onActivateCard
                },
                'aduser #detailform': {
                    activate: this.onActivateCard
                },
                'aduser #detail': {
                    click: this.switchToDetailForm
                },
                'aduser #table': {
                    click: this.onTableButtonClick
                },
                'aduser #prev': {
                    click: this.onPrevButtonClick
                },
                'aduser #next': {
                    click: this.onNextButtonClick
                },
                //

                'aduser #extfilterbutton': {
                    click: this.onFilterButtonClick
                },

                'aduser #refresh': {
                    click: this.onRefreshButtonClick
                },
                'aduser #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});