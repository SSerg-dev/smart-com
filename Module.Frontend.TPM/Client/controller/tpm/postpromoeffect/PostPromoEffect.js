Ext.define('App.controller.tpm.postpromoeffect.PostPromoEffect', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'postpromoeffect[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'postpromoeffect directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'postpromoeffect #datatable': {
                    activate: this.onActivateCard
                },
                'postpromoeffect #detailform': {
                    activate: this.onActivateCard
                },
                'postpromoeffect #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'postpromoeffect #detailform #next': {
                    click: this.onNextButtonClick
                },
                'postpromoeffect #detail': {
                    click: this.onDetailButtonClick
                },
                'postpromoeffect #table': {
                    click: this.onTableButtonClick
                },
                'postpromoeffect #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'postpromoeffect #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'postpromoeffect #createbutton': {
                    click: this.onCreateButtonClick
                },
                'postpromoeffect #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'postpromoeffect #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'postpromoeffect #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'postpromoeffect #refresh': {
                    click: this.onRefreshButtonClick
                },
                'postpromoeffect #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'postpromoeffect #exportbutton': {
                    click: this.onExportButtonClick
                },
                'postpromoeffect #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'postpromoeffect #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'postpromoeffect #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },

                'postpromoeffecteditor numberfield[name=EffectWeek1]': {
                    change: this.onEffectWeekChange
                },
                'postpromoeffecteditor numberfield[name=EffectWeek2]': {
                    change: this.onEffectWeekChange
                }
            }
        });
    },

    // total effect = effect week 1 + effect week 2
    onEffectWeekChange: function (field, newValue, oldValue) {
        var effectWeek1 = field.up().down('[name=EffectWeek1]').getValue();
        var effectWeek2 = field.up().down('[name=EffectWeek2]').getValue();
        var totalEffectField = field.up().down('[name=TotalEffect]');

        totalEffectField.setValue(effectWeek1 + effectWeek2);
    }
});
