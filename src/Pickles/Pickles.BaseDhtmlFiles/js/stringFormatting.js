function addSpacesToCamelCasedString(unformattedString) {
    // IE does not implement trim() and the following replaces the functionality if unavailable
    // http://stackoverflow.com/questions/2308134/trim-in-javascript-not-working-in-ie
    if (typeof String.prototype.trim !== 'function') {
        String.prototype.trim = function() {
            return this.replace(/^\s+|\s+$/g, '');
        }
    }
    
    return unformattedString.replace(/([A-Z])/g, " $1").trim();
}

function removeBeginningHash(hashedString) {
    if (hashedString.substring(0, 1) == "#") {
        return hashedString.substring(1, hashedString.length);
    } else {
        return hashedString;
    }
}