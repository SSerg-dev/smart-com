CustomValidators.define([{
    name: 'StartDateFinishDateOrder',
    message: l10n.ns('core', 'customValidators').value('StartDateFinishDateOrder'),
    fields: ['StartDate', 'FinishDate'],
    isValid: function (model) {
        var startDate = model.get('StartDate'),
            finishDate = model.get('FinishDate');

        return startDate.getTime() < finishDate.getTime();
    }
}]);