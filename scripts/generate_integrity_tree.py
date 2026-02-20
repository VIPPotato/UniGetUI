import hashlib
import json
import os
import sys


def main() -> int:
    if len(sys.argv) < 2:
        raise ValueError("A directory path must be provided as an argument.")

    root_dir: str = os.path.abspath(sys.argv[1])
    if not os.path.isdir(root_dir):
        raise FileNotFoundError(f"The directory '{root_dir}' does not exist.")

    integrity_data: dict[str, str] = {}
    script_name = os.path.basename(__file__)
    output_filename = "IntegrityTree.json"
    min_output = "--min-output" in sys.argv

    for subdir, _, files in os.walk(root_dir):
        for filename in files:
            if filename == script_name or filename == output_filename:
                continue

            file_path = os.path.join(subdir, filename)
            relative_path = os.path.relpath(file_path, root_dir).replace("\\", "/")
            # Exclude volatile runtime/user-data folders from integrity snapshots.
            lowered = relative_path.lower()
            if lowered.startswith("settings/") or lowered.startswith("publish/"):
                continue
            if not min_output:
                print(f" - Computing MD5SUM of {relative_path}...")

            with open(file_path, "rb") as file_stream:
                md5_hash = hashlib.md5(file_stream.read())
            integrity_data[relative_path] = md5_hash.hexdigest()

    output_file_path = os.path.join(root_dir, output_filename)
    with open(output_file_path, "w", encoding="utf-8") as file_stream:
        json.dump(integrity_data, file_stream, indent=4, sort_keys=True)

    print(f"Integrity tree was generated and saved to {root_dir.rstrip('/\\')}/{output_filename}")
    return 0


if __name__ == "__main__":
    try:
        sys.exit(main())
    except Exception as ex:
        print(ex, file=sys.stderr)
        sys.exit(1)
