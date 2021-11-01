Ext.define('App.view.tpm.common.competitorPromoDashboardTpl', {

    formatTpl: new Ext.XTemplate([
    '<div id="promo-rowexpander-wrap" style="width: 990px; height: 100%;">',
        // Basic
        '<div class="promo-rowexpander-frame" style="padding: 0px 5px 0px 0px">',
            '<div class="custom-promo-panel" style="margin-left: 12px; width: 385px; height: 150px;">',
                '<table>',
                    '<tr>',
                        // Name + Status
                        '<td>',
                            '<b data-qtip="{[this.rowFormat(values.Name)]}" style="text-overflow: ellipsis; overflow: hidden; display: inline-block; max-width: 119px; white-space: nowrap;">{[this.rowFormat(values.Name)]}</b><b style="text-overflow: ellipsis; overflow: hidden; display: inline-block; white-space: nowrap; margin-left: 5px">(ID: {[values.Number]})</b></br>',
                            '<div class="border-left-box promo-grid-row" style="height: 18px; border-color: {[values.PromoStatusColor]};">',
                                '<span style="text-align: left;"><b>Status:</b> {[values.Status]}</br>',
                            '</div>',
                        '</td>',
                    '</tr>',
                    
                '</table>',
            '</div>',
        '</div>',
    '</div>',
    {
        formatDate: function (value) {
            return Ext.util.Format.date(value, 'd.m.Y');
        },
        rowFormat: function (value) {
            if (value) {
                return renderWithDelimiter(value, ' > ', '  ');
            }
            else {
                return '-';
            }     
		},
		tipFormat: function (value) {
			return value;
		},
        simpleFormat: function (value) {
            return value ? Ext.util.Format.number(value, '0.00') : '-';
        },
        millionFormat: function (value) {
            var result = '-';            

            if (value !== null && value !== undefined) {
                var valueToDisplay = null;

                if (this.blockBillion) {
                    valueToDisplay = value;
                }
                else {
                    this.originValue = value;
                    valueToDisplay = value / 1000000.0;
                }

                result = Ext.util.Format.number(valueToDisplay, '0.00');
            }

            return result;
        },
        isDisplayPlan: function (value) {
            return value == 'Closed' ? 'display: none;' : '';
        },
        isDisplayFact: function (value) {
            return value == 'Closed' ? '' : 'display: none;';
        },
        isEmptyValueColor: function (value) {
            return value !== null && value !== undefined ? '#4CAF50' : '#F44336';
        },
        isEmptyValue: function (value) {
            return value !== null && value !== undefined ? 'Yes' : 'No';
		}
    }
    ])
});