Ext.define('App.view.tpm.competitorbrandtech.CompetitorBrandTechEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.competitorbrandtecheditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,
    cls: 'readOnlyFields',

    afterWindowShow: function () {
        this.down('circlecolorfield[name=Color]').focus(true, 10);
    },

    items: {
        xtype: 'editorform',
        items: [{
            xtype: 'circlecolorfield',
            name: 'Color',
            fieldLabel: l10n.ns('tpm', 'CompetitorBrandTech').value('Color'),
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'CompetitorBrandTech').value('CompetitorName'),
            name: 'CompetitorId',
            selectorWidget: 'competitor',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.competitor.Competitor',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.competitor.Competitor',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'CompetitorName'
            }]
        }, {
            xtype: 'textfield',
            fieldLabel: l10n.ns('tpm', 'CompetitorBrandTech').value('BrandTech'),
            name: 'BrandTech',
        }]
    }
});
