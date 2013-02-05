function addSpacesToCamelCasedString(unformattedString) {
    return unformattedString.replace(/([A-Z])/g, " $1").trim();
}

function removeBeginningHash(hashedString) {
    if (hashedString.substring(0, 1) == "#") {
        return hashedString.substring(1, hashedString.length);
    } else {
        return hashedString;
    }
}