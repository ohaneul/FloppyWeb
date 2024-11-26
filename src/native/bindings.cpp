#include <emscripten/bind.h>

using namespace emscripten;

EMSCRIPTEN_BINDINGS(floppy_web) {
    class_<URLParser>("URLParser")
        .constructor<>()
        .function("isValidURL", &URLParser::isValidURL);

    class_<HistoryManager>("HistoryManager")
        .constructor<>()
        .function("addEntry", &HistoryManager::addEntry)
        .function("getSuggestions", &HistoryManager::getSuggestions);

    class_<SearchEngine>("SearchEngine")
        .constructor<>()
        .function("findInPage", &SearchEngine::findInPage);
} 