Ext.define('App.view.tpm.coefficientsi2so.CustomMarsDateField', {
    extend: 'App.view.core.common.MarsDateField',
    alias: 'widget.custommarsdatefield',

  
    onTriggerClick: function () {
        var picker = Ext.widget('marsdatepicker', {
            pickerField: this,
            weekRequired: this.weekRequired,

            listeners: {
                scope: this,
                select: this.onSelect
            }

        });

        var cbWeekLabel = picker.down('[forId=cbWeek]');
        cbWeekLabel.setVisible(false);
        picker.cbWeek.setVisible(false);

        var val = this.getValue();
        if (!val) {
            var now = new App.MarsDate();
            val = new App.MarsDate(now.getYear(), now.getPeriod(), undefined);
        }

        picker.setValue(val);
        picker.show();
    },

});