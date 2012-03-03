(function ($, ko, window) {

    window.Steeple = window.Steeple || {};

    window.Steeple.SteepleModel = function () {

        var self = this;

        self.uimode = ko.observable(Steeple.UIModes.PrayerList);
        self.currentPrayer = null;

        self.prayers = ko.observableArray([
            new Steeple.Prayer('prayer1')
        ]);

        self.addPrayer = function () {
            self.prayers.push(new Steeple.Prayer('New Prayer'));
            self.uimode(Steeple.UIModes.PrayerView);
        };
    }

} (jQuery, ko, window));