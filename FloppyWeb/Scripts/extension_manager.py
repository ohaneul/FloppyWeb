import asyncio
from typing import Dict, List
import json
from pathlib import Path

class ExtensionManager:
    def __init__(self):
        self.extensions: Dict[str, Extension] = {}
        self.event_loop = asyncio.get_event_loop()
        self.extension_api = ExtensionAPI()

    async def load_extensions(self, extensions_dir: Path):
        """Load all extensions in parallel"""
        extension_paths = list(extensions_dir.glob("*/manifest.json"))
        load_tasks = [self.load_extension(path) for path in extension_paths]
        await asyncio.gather(*load_tasks)

    async def load_extension(self, manifest_path: Path):
        try:
            manifest = json.loads(manifest_path.read_text())
            extension = Extension(manifest, self.extension_api)
            await extension.initialize()
            self.extensions[extension.id] = extension
        except Exception as e:
            print(f"Failed to load extension {manifest_path}: {e}")

    async def execute_extensions(self, event_type: str, data: dict):
        """Execute all extensions for an event in parallel"""
        tasks = [
            ext.handle_event(event_type, data)
            for ext in self.extensions.values()
            if ext.handles_event(event_type)
        ]
        await asyncio.gather(*tasks) 