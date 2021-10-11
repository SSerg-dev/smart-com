Ext.define('App.view.tpm.calendarcompetitorbrandtechcolor.CalendarCompetitorBrandTechColorEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.calendarcompetitorbrandtechcoloreditor',
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
            fieldLabel: l10n.ns('tpm', 'CalendarCompetitorBrandTechColor').value('Color'),
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'CalendarCompetitorBrandTechColor').value('CompanyName'),
            allowBlank: true,
            allowOnlyWhitespace: true,
            name: 'CompanyName',
            selectorWidget: 'companyname',
            valueField: 'Id',
            displayField: 'CompanyName',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.calendarcompetitorcompany.CalendarCompetitorCompany',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.calendarcompetitorcompany.CalendarCompetitorCompany',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'CompanyName',
                to: 'CompanyName'
            }]
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'CalendarCompetitorBrandTechColor').value('BrandTech'),
            name: 'BrandTech',
        }]
    }
});
