Ext.define('App.view.tpm.incrementalpromo.IncrementalPromoEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.incrementalpromoeditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,
    cls: 'readOnlyFields',

    afterWindowShow: function (scope, isCreating) {
        scope.down('numberfield[name=PlanPromoIncrementalCases]').focus(true, 10);
    },

    items: {
        xtype: 'editorform',
        columnsCount: 2,
        items: [{
            xtype: 'textfield',
            name: 'ProductZREP',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('ProductZREP'),
            readOnlyCls: 'readOnlyField',
            readOnly: true,
            setReadOnly: function () { return false },
        }, {
            xtype: 'textfield',
            name: 'ProductName',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('ProductName'),
            readOnlyCls: 'readOnlyField',
            readOnly: true,
            setReadOnly: function () { return false },
        }, {
            xtype: 'textfield',
            name: 'PromoClient',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoClient'),
            readOnlyCls: 'readOnlyField',
            readOnly: true,
            setReadOnly: function () { return false },
        }, {
            xtype: 'textfield',
            name: 'PromoNumber',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoNumber'),
            readOnlyCls: 'readOnlyField',
            readOnly: true,
            setReadOnly: function () { return false },
        }, {
            xtype: 'textfield',
            name: 'PromoName',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PromoName'),
            readOnlyCls: 'readOnlyField',
            readOnly: true,
            setReadOnly: function () { return false },
        }, {
            xtype: 'numberfield',
            name: 'PlanPromoIncrementalCases',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PlanPromoIncrementalCases'),
            allowBlank: false,
            minValue: 0,
            maxValue: 100000000000000000000,
            listeners: {
                change: function (field, newVal, oldVal) {
                    IncrementalPromo_SetPlanLSV(field.up('editorform'));
                }
            }
        }, {
            xtype: 'numberfield',
            name: 'CasePrice',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('CasePrice'),
            allowBlank: false,
            minValue: 0,
            maxValue: 100000000000000000000,
            listeners: {
                change: function (field, newVal, oldVal) {
                    IncrementalPromo_SetPlanLSV(field.up('editorform'));
                }
            }
        }, {
            xtype: 'numberfield',
            name: 'PlanPromoIncrementalLSV',
            fieldLabel: l10n.ns('tpm', 'IncrementalPromo').value('PlanPromoIncrementalLSV'),
            readOnlyCls: 'readOnlyField',
            readOnly: true,
            setReadOnly: function () { return false },
        }]
    }
});

function IncrementalPromo_SetPlanLSV(form) {
    var casesField = form.down('numberfield[name=PlanPromoIncrementalCases]');
    var casePriceField = form.down('numberfield[name=CasePrice]');
    var lsvField = form.down('numberfield[name=PlanPromoIncrementalLSV]');

    var cases = casesField.getValue();
    var casePrice = casePriceField.getValue();
    var lsv = cases !== null && casePrice !== null ? cases * casePrice : null;

    lsvField.setValue(lsv);
}